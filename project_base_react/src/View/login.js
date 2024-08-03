
import React from 'react';
import { Button, Card, CardBody, CardFooter, CardHeader, Container } from 'reactstrap';
import InputField from '../Control/MyInputField';
import './base.css'
import { httpPost } from '../Comon/baseApi';
export default function Login() {
    const handleLogin = () => {

    }

    return (
        <>
            <Container className="d-flex justify-content-center align-items-center min-vh-100">
                <Card className="centered-card">
                    <CardHeader>
                        <div>Login Page</div>
                    </CardHeader>
                    <CardBody>
                        <InputField type='text' label="username" />
                        <InputField type='password' label="password" />
                    </CardBody>
                    <CardFooter>
                        <Button color='success' className='float-right' onClick={() => handleLogin()}>
                            Login
                        </Button>
                    </CardFooter>
                </Card>
            </Container>

        </>
    );
}
