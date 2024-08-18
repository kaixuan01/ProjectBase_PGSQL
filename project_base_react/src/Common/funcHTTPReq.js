const FuncHTTPReq = async ({
    method = 'GET',
    url,
    baseUrl = 'https://localhost:7032',
    data = null,
    credentials = 'include',
    headers = {},
    responseType = 'json',
    onSuccess,
    onError
}) => {
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
            throw new Error(`HTTP error! status: ${response.status}`);
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
            // You can define showErrorAlert or handle errors here
            console.error(result.message);
        }
    } catch (error) {
        if (onError) {
            onError(error);
        } else {
            console.error('HTTP Request Failed:', error);
        }
    }
};

export default FuncHTTPReq;