const axios = require("axios");
const FormData = require("form-data");
const qs = require("qs");
const fs = require("fs");
const https = require("https");

async function authorize(
  identityURL,
  clientID,
  clientSecret,
  username,
  password,
  grantType,
  apiKey
) {
  let data = qs.stringify({
    client_id: clientID,
    username: username,
    password: password,
    grant_type: grantType,
    client_secret: clientSecret,
  });

  try {
    console.log("Sending Request to Authorize");
    response = await axios.post(identityURL, data, {
      headers: {
        "x-api-key": apiKey,
        "Content-Type": "application/x-www-form-urlencoded",
      },
      httpsAgent: new https.Agent({ rejectUnauthorized: false }),
    });
    if (response.status == 200) {
      console.log("Authorized Successfully");
      return response.data.access_token;
    } else {
      console.log("Authorization Failed");
      return null;
    }
  } catch (error) {
    console.log(error);
    return null;
  }
}

async function classify(cdcURL, accessToken, apiKey, documentPath) {
  let data = new FormData();
  data.append("document", fs.createReadStream(documentPath));
  data.append("metadata", '{ "IHXPayerID": "<payer-id>"}');
  data.append(
    "process_info",
    '{"KYC_Classification": true,"Page_Error": "IGNORE"}'
  );

  try {
    console.log("Sending Request to Classify");
    response = await axios.post(cdcURL, data, {
      headers: {
        "Content-Type": "multipart/form-data",
        Accept: "application/json",
        "access-token": accessToken,
        "x-api-key": apiKey,
        ...data.getHeaders(),
      },
      httpsAgent: new https.Agent({ rejectUnauthorized: false }),
    });
    if (response.status == 200) {
      console.log("Classification Successful");
      return response.data;
    } else {
      console.log("Classification Failed");
      return null;
    }
  } catch (error) {
    console.log(error);
    return null;
  }
}

async function main(documentPath) {
  const clientID = "<client-id>";
  const clientSecret = "<client-secret>";
  const username = "<username>";
  const password = "<password>";
  const grantType = "<grant-type>";

  const identityURL = "https://<identity-url>";

  const cdcURL = "https://<cdc-url>";

  const apiKey = "<api-key>";

  try {
    accessToken = await authorize(
      identityURL,
      clientID,
      clientSecret,
      username,
      password,
      grantType,
      apiKey
    );
    if (accessToken != null) {
      response = await classify(cdcURL, accessToken, apiKey, documentPath);
      if (response != null) {
        return JSON.stringify(response);
      } else {
        return null;
      }
    }
  } catch (error) {
    console.log(error);
    return null;
  }
}

let response = new Promise((resolve, reject) => {
  try {
    resolve(main("<path-to-document>"));
  } catch (error) {
    reject(error);
  }
});

response.then((value) => {
  console.log(value);
});
