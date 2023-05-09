import requests

url = "https://<url-to-cdc>"

payload = {'metadata': '{ "IHXPayerID": "000"}',
'process_info': '{"KYC_Classification": true,"Page_Error": "IGNORE"}'}
files=[
  ('document',('<name>.pdf',open('<path-to-file>.pdf','rb'),'application/pdf'))
]
headers = {
  'Content-Type': 'multipart/form-data',
  'Accept': 'application/json',
  'x-api-key': '<api-key>'
}

response = requests.request("POST", url, headers=headers, data=payload, files=files)

print(response.text)