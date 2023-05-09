var myHeaders = new Headers();
myHeaders.append("Content-Type", "multipart/form-data");
myHeaders.append("Accept", "application/json");
myHeaders.append("x-api-key", "<api-key>");

var formdata = new FormData();
formdata.append("document", fileInput.files[0], "<path-to-file>.pdf");
formdata.append("metadata", "{ \"IHXPayerID\": \"000\"}");
formdata.append("process_info", "{\"KYC_Classification\": true,\"Page_Error\": \"IGNORE\"}");

var requestOptions = {
    method: 'POST',
    headers: myHeaders,
    body: formdata,
    redirect: 'follow'
};

fetch("https://<url-to-cdc>", requestOptions)
    .then(response => response.text())
    .then(result => console.log(result))
    .catch(error => console.log('error', error));