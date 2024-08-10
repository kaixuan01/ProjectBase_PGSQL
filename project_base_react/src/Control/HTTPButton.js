import React from 'react';
import { Button } from 'reactstrap';


const HTTPButton = ({ method = 'GET', url, data = null, onSuccess, onError, children, ...props }) => {
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
            debugger;
            const response = await fetch("https://localhost:7032" + url, options);

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
        }
    };

    return (
        <Button onClick={handleClick} {...props}>
            {children}
        </Button>
    );
};

export default HTTPButton;
