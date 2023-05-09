const axios = require('axios');
const FormData = require('form-data');
const fs = require('fs');
let data = new FormData();
data.append('document', fs.createReadStream('<path-to-file>.pdf'));
data.append('metadata', '{ "IHXPayerID": "000"}');
data.append('process_info', '{"KYC_Classification": true,"Page_Error": "IGNORE"}');

let config = {
  method: 'post',
  maxBodyLength: Infinity,
  url: 'https://<url-to-cdc>',
  headers: { 
    'Content-Type': 'multipart/form-data', 
    'Accept': 'application/json', 
    'x-api-key': 'api-key', 
    ...data.getHeaders()
  },
  data : data
};

axios.request(config)
.then((response) => {
  console.log(JSON.stringify(response.data));
})
.catch((error) => {
  console.log(error);
});