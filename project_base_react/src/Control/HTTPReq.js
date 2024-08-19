import { showErrorAlert } from "../Common/common";
import Cookies from 'js-cookie';

const HTTPReq = ({
    method = 'GET',
    url,
    baseUrl = 'https://localhost:7032',
    data = null,
    credentials = 'include',
    headers = {},
    responseType = 'json',
    onSuccess,
    onError,
    children,
    ...props
}) => {
    const sendRequest = async () => {
        try {
            let options = {
                method: method,
                credentials: credentials,
                headers: {
                    'Content-Type': 'application/json',
                    ...headers,
                },
            };

            if (['POST', 'PUT', 'PATCH'].includes(method.toUpperCase()) && data) {
                options.body = JSON.stringify(data);
            }

            const response = await fetch(baseUrl + url, options);

            if (!response.ok) {
                if (response.status === 401) {
                    // Check if the 'IsBlock' cookie exists and its value is 'true'
                    const isBlocked = Cookies.get('isBlocked');
                    console.log(isBlocked);
                    if (isBlocked && isBlocked.split('=')[1] === 'true') {
                        showErrorAlert("Your account has been blocked, please contact admin to proceed.");
                    }else{
                        showErrorAlert("Your session is expired. Please login again.");
                    }
                    
                    // Set isLogin = false
                    
                }else if(response.status === 403){
                    showErrorAlert("You have no permission on this function!");
                }else {
                    showErrorAlert("Service temporary not available. Please try again later.");
                }
            }

            let result;
            if (responseType === 'json') {
                result = await response.json();
            } else if (responseType === 'text') {
                result = await response.text();
            } else {
                result = await response.blob();
            }
            
            if (responseType === 'json' && result.success) {
                if (onSuccess) {
                    const data = {
                        ...result.data,
                        message: result.message
                    };
                    onSuccess(data);
                }
            } else {
                showErrorAlert(result.message);
            }
        } catch (error) {
            if (onError) {
                onError(error);
            } else {
                showErrorAlert(error);
                console.error('HTTP Request Failed:', error);
            }
        }
    };

    return children({ sendRequest });
};

export default HTTPReq;
