import React from 'react';
import { Button } from 'reactstrap';


const HTTPButton = ({ method = 'GET', url, data = null, onSuccess, onError, children, ...props }) => {
    const handleClick = async () => {
        try {
            let options = {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                },
            };

            if (method === 'POST' && data) {
                options.body = JSON.stringify(data);
            }

            const response = await fetch(url, options);

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
