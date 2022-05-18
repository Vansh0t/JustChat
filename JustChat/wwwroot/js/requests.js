const REFRESH_TOKEN_ENDPOINT = "/auth/jwt/refresh"


const httpClient = axios.create();
httpClient.defaults.headers.post['Content-Type'] = 'application/json';

var isAuthenticated = ($('#isAuthenticated').val().toLowerCase()==="true")
console.log(isAuthenticated)

//try silent sign in with refresh token
if(!isAuthenticated && !window.sessionStorage.getItem("refreshAttemptFailed")) {
    httpClient.post(REFRESH_TOKEN_ENDPOINT, {})
        .then(response=>{
            console.log(response)
            location.reload()
        })
        .catch(error=>{
            if(error.response.status == 403)
                window.sessionStorage.setItem("refreshAttemptFailed", "true")
        })
}





const postForm = (formId, url, onSuccess)=>{
    const form = $('#'+formId)
    const errorMsg = $('#'+formId + ' p')
    errorMsg.attr('hidden', true)
    var isFormValid = form.get(0).reportValidity();
    if(!isFormValid) return;
    var formData = {}
    form.serializeArray().forEach(inputData => {
        formData[inputData.name] = inputData.value
    });
    httpClient.post(url, formData).
        then((response)=>{
            if(onSuccess)
                onSuccess(response)
        }).catch((error)=>{
            console.log(error)
            errorMsg.text(error.response.data)
            errorMsg.attr('hidden', false)
        })
}