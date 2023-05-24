import requests
from urllib.parse import quote_plus


# API for token generation
def authorize(
    identity_url, client_id, client_secret, username, password, grant_type, api_key
):
    """
    This function generates the access token for the user
    :param identity_url: Identity URL
    :param client_id: Client ID
    :param client_secret: Client Secret
    :param username: Username
    :param password: Password
    :param grant_type: Grant Type
    :param api_key: API Key
    :return: Access Token
    """
    print("Calling Authorizer")
    try:
        # need to escape special characters in password (if any)
        password = quote_plus(password)
        payload = f"client_id={client_id}&client_secret={client_secret}&username={username}&password={password}&grant_type={grant_type}"
        headers = {
            "x-api-key": api_key,
            "Content-Type": "application/x-www-form-urlencoded",
        }

        response = requests.request("POST", identity_url, headers=headers, data=payload,verify=False)
        print("Authorizer Response Code")
        print(response.status_code)
        if response.status_code == 200:
            return response.json()["access_token"]
        else:
            return None
    except Exception as e:
        print(e)
        return None


# DC Call API
def call_dc_api(url, access_token, api_key, payload, file_path):
    """
    This function calls the DC API
    :param url: DC URL
    :param access_token: Access Token
    :param api_key: API Key
    :param payload: Payload
    :param file_path: File Path
    :return: Response - JSON : Classification Result
    """
    print("Calling DC")
    try:
        file_name = file_path.split("/")[-1]
        files = [
            (
                "document",
                (
                    file_name,
                    open(
                        file_path,
                        "rb",
                    ),
                    "application/pdf",
                ),
            )
        ]
        headers = {
            "Accept": "application/json",
            "x-api-key": api_key,
            "access-token": access_token,
        }

        response = requests.request(
            "POST", url, headers=headers, data=payload, files=files,verify=False
        )
        if response.status_code == 200:
            print("Calling DC Successful")
            return response.json()
        else:
            print("Calling DC Failed")
            return None
    except Exception as e:
        print(e)
        return None


def execute_dc(file_path):
    """
    This function executes the DC API
    :param file_path: File Path
    :return: Response - JSON : Classification Result
    """
    client_id = "<client-id>"
    client_secret = "<client-secret>"
    username = "<username>"
    password = "<password>"
    grant_type = "<grant-type>"
    identity_url = "https://<identity-url>"
    dc_url = "https://<dc-url>"
    api_key = "api-key"
    payload = {
        "metadata": '{ "IHXPayerID": "<payer-id>"}',
        "process_info": '{"KYC_Classification": true,"Page_Error": "IGNORE"}',
    }
    try:
        access_token = authorize(
            identity_url,
            client_id,
            client_secret,
            username,
            password,
            grant_type,
            api_key,
        )
        if access_token is not None:
            return call_dc_api(dc_url, access_token, api_key, payload, file_path)
        else:
            return None
    except Exception as e:
        print(e)
        return None


dc_response = execute_dc("path-to-file")
print(dc_response)
