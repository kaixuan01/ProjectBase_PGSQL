import React, { useCallback, useState } from 'react';
import { Card, CardBody, CardFooter, CardHeader, Container, Button } from 'reactstrap';
import '../CSS/base.css'
import HTTPReq from '../Control/HTTPReq';
import MyInputField from '../Control/MyInputField';
import { showSuccessAlert } from '../Common/common';

export default function Login({onLogin}) {
    const [username, SetUsername] = useState('');
    const [password, SetPassword] = useState('');

    const successLogin = useCallback((result) => {
        showSuccessAlert(result.message);
        onLogin();
    }, []);

    return (
        <Container className="d-flex justify-content-center align-items-center min-vh-100">
            <Card className="centered-card">
                <CardHeader>
                    <div>Login Page</div>
                </CardHeader>
                <CardBody>
                    <MyInputField label="username" onChange={(e) => SetUsername(e.target.value)} />
                    <MyInputField label="password" type="password" onChange={(e) => SetPassword(e.target.value)} />
                </CardBody>
                <CardFooter>
                    <HTTPReq
                        method="POST"
                        url={`/OAuth`}
                        credentials='include'
                        onSuccess={(result) => successLogin(result)}
                        data={{username, password}}
                    >
                        {({ sendRequest }) => (
                            <Button onClick={sendRequest} color="success" className="float-right">Login</Button>
                        )}
                    </HTTPReq>
                </CardFooter>
            </Card>
        </Container>
    );
}
