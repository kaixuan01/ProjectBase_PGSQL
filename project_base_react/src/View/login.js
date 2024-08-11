
import React, { useState } from 'react';
import { Card, CardBody, CardFooter, CardHeader, Container, Button } from 'reactstrap';
import './base.css';
import HTTPReq from '../Control/HTTPReq';
import MyInputField from '../Control/MyInputField'
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
                        <MyInputField label="password" onChange={(e) => SetPassword(e.target.value)}></MyInputField>
                    </CardBody>
                    <CardFooter>
                    <HTTPReq
                        method="GET"
                        url={`/OAuth?username=${username}&password=${password}`}
                        onSuccess={(result) =>  console.log(result)}
                        credentials='omit'
                    >
                        {({ handleClick }) => (
                            <Button onClick={handleClick} color="success" className="float-right">Login</Button>
                        )}
                    </HTTPReq>
                    </CardFooter>
                </Card>
            </Container>
    );
}
