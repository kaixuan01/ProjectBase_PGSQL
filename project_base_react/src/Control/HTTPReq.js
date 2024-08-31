import { showErrorAlert  } from "../Common/Common";
import { useFuncHTTPReq } from "../Hook/FuncHttpReq";
const HTTPReq = ({
    method = 'GET',
    url,
    baseUrl = 'http://localhost:2030',
    data = null,
    credentials = 'include',
    headers = {},
    responseType = 'json',
    onSuccess,
    onError,
    children,
    ...props
}) => {
    const { FuncHTTPReq } = useFuncHTTPReq();
    const sendRequest = async () => {
        try {
            await FuncHTTPReq({
                method,
                url,
                baseUrl,
                data,
                credentials,
                headers,
                responseType,
                onSuccess: (data) => {
                    if (onSuccess) {
                        onSuccess(data);
                    }
                },
                onError: (error) => {
                    if (onError) {
                        onError(error);
                    } else {
                        showErrorAlert(error.message || "An error occurred");
                    }
                }
            });
        } catch (error) {
            console.error('HTTP Request Failed:', error);
            showErrorAlert(error.message || "An error occurred");
        }
    };

    return children({ sendRequest });
};

export default HTTPReq;