const REFRESH_TOKEN_ENDPOINT = "/auth/jwt/refresh"
const SIGNOUT_ENDPOINT = "/auth/signout"
const REFRESH_EXPIRATION_DIFF = 10000 //the refresh attempt will happen in (expiration-this_value) ms


var isAuthenticated = false;

const httpClient = axios.create();
httpClient.defaults.headers.post['Content-Type'] = 'application/json';

//var isAuthenticated = ($('#isAuthenticated').val().toLowerCase()==="true")
//console.log(isAuthenticated)
const getUtcEpochMsNow = () => {
    return  Date.parse(new Date().toISOString())
}
const getUtcEpochMs = (time) => {
    return  Date.parse(new Date(time).toISOString())
}

const onAuthSuccess = (resp) => {
    window.localStorage.setItem('refreshAfter', resp.data.jwt.expiration-REFRESH_EXPIRATION_DIFF)
    isAuthenticated = true
    setRefreshTimeout()
    
}
const onRefreshSuccess = (resp) => {
    window.localStorage.setItem('refreshAfter', resp.data.expiration-REFRESH_EXPIRATION_DIFF)
    isAuthenticated = true;
    setRefreshTimeout()
}


const refreshJwt = () => {
    isAuthenticated = false;
    httpClient.post(REFRESH_TOKEN_ENDPOINT, {})
        .then(resp=>{
            onRefreshSuccess(resp)
            console.debug("jwt refreshed", resp)
        })
        .catch(error=>{
            if(error.response.status == 403)
                window.sessionStorage.setItem("refreshAttemptFailed", "true")
        })
}

const setRefreshTimeout = () => {
    const utcEpochMsNow = Date.parse(new Date().toISOString())
    const expiration = window.localStorage.getItem('refreshAfter')
    console.debug("refresh timeout set: ",  expiration-utcEpochMsNow)
    setTimeout(()=> {
        try {
            refreshJwt()
        } catch (error) {
            console.error(error)
        }
    }, expiration-utcEpochMsNow)
}


const refreshAfter = window.localStorage.getItem("refreshAfter")
//try silent sign in with refresh token
if(refreshAfter) {
    if(getUtcEpochMsNow()>=refreshAfter) {
        refreshJwt()
    }
    else {
        setRefreshTimeout()
        isAuthenticated = true;
    }
}



const signout = ()=> {
    httpClient.post(SIGNOUT_ENDPOINT)
        .then(resp=>{
            window.localStorage.removeItem("refreshAfter")
            console.debug("signed out", resp)
            location.reload();
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
            try {
                if(onSuccess)
                    onSuccess(response)
            } catch (error) {
                console.error(error)
            }
            
        }).catch((error)=>{
            console.log(error)
            errorMsg.text(error.response.data)
            errorMsg.attr('hidden', false)
        })
}