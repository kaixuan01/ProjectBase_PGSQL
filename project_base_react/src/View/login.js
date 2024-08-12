
import React, { useState } from 'react';
import { Card, CardBody, CardFooter, CardHeader, Container, Button } from 'reactstrap';
import './base.css';
import HTTPReq from '../Control/HTTPReq';
import MyInputField from '../Control/MyInputField'
import { showSuccessAlert } from '../Comon/common';
export default function Login() {
    const [ username, SetUsername ] = useState('');
    const [ password, SetPassword ] = useState('');
    return (
            <Container className="d-flex justify-content-center align-items-center min-vh-100">
                <Card className="centered-card">
                    <CardHeader>
                        <div>Login Page</div>
                    </CardHeader>
                    <CardBody>
                        <MyInputField label="username" onChange={(e) => SetUsername(e.target.value)}></MyInputField>
                        <MyInputField label="password" type="password" onChange={(e) => SetPassword(e.target.value)}></MyInputField>
                    </CardBody>
                    <CardFooter>
                    <HTTPReq
                        method="POST"
                        url={`/OAuth`}
                        credentials='omit'
                        onSuccess={showSuccessAlert("Login Sucessfully!")}
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
