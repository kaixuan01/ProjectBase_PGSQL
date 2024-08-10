
import React, { useState } from 'react';
import { Card, CardBody, CardFooter, CardHeader, Container } from 'reactstrap';
import InputField from '../Control/MyInputField';
import './base.css'
import HTTPButton from '../Control/HTTPButton';

export default function Login() {
    const [ username, SetUsername ] = useState('');
    const [ password, SetPassword ] = useState('');

    return (
        <>
            <Container className="d-flex justify-content-center align-items-center min-vh-100">
                <Card className="centered-card">
                    <CardHeader>
                        <div>Login Page</div>
                    </CardHeader>
                    <CardBody>
                        <InputField type='text' label="username" onChange={(e) => {SetUsername(e.target.value)}} />
                        <InputField type='password' label="password" onChange={(e) => {SetPassword(e.target.value)}} />
                    </CardBody>
                    <CardFooter>
                    <HTTPButton
                        method="POST"
                        url="/submit"
                        data={{ username, password }}
                        onSuccess={(result) => console.log(result)}
                        onError={(error) => console.error(error)}
                        color="success"
                        className="float-right"
                    >
                        Login
                    </HTTPButton>

                    </CardFooter>
                </Card>
            </Container>

        </>
    );
}
