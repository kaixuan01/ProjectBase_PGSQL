import { useAuthHandlers } from "./AuthHandlers";
import Cookies from "js-cookie";
import { showErrorAlert } from "../Common/Common";
export const useFuncHTTPReq = () => {
  const { handleLogout } = useAuthHandlers();

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
      const isBlocked = Cookies.get('isBlocked');
      if (!response.ok) {
        if (response.status === 401) {
          console.log(isBlocked);
          if (isBlocked === 'true') {
            showErrorAlert("Your account has been blocked, please contact admin to proceed.");
          } else {
            showErrorAlert("Your session is expired. Please login again.");
          }
          handleLogout(); // Safe to use handleLogout here
        } else if (response.status === 403) {
          showErrorAlert("You have no permission on this function!");
        } else {
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
          // const data = {
          //   ...result.data,
          //   message: result.message
          // };
          onSuccess(result.data, result.message);
        }
      } else {
        showErrorAlert(result.message);
        console.error(result.message);
      }
    } catch (error) {
      if (onError) {
        showErrorAlert(error);
        onError(error);
      } else {
        console.error('HTTP Request Failed:', error);
      }
    }
  };

  return { FuncHTTPReq };
};
