const HTTPReq = ({ method = 'GET', url, data = null, credentials = 'include', onSuccess, onError, children, ...props }) => {
    const handleClick = async () => {
        try {
            let options = {
                method: method,
            };

            if (method === 'POST' && data) {
                options.body = JSON.stringify(data);
                options.headers = {
                    'Content-Type': 'application/json',
                }
            }
            const response = await fetch("https://localhost:7032" + url, options);
            debugger;
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (onSuccess) {
                onSuccess(result);
            }
        } catch (error) {
            if (onError) {
                onError(error);
            }
            else{
                console.error(error);
            }
        }
    };

    return children({ handleClick });
};

export default HTTPReq;