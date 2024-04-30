import './Login.css';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios, { AxiosError } from 'axios';
import React, { useRef, useState } from 'react';
import { useNavigate } from 'react-router-dom';

export default function Login() {
    const minEmailLength: number = 5;
    const maxEmailLength: number = 256;
    const minPasswordLength: number = 8;
    const maxPasswordLength: number = 20;
    const urlRegister: string = process.env.REACT_APP_API_URL + 'login';

    const inputEmail = useRef<HTMLInputElement>(null);
    const inputPassword = useRef<HTMLInputElement>(null);
    const form = useRef<HTMLFormElement>(null);

    const accessTokenKey: string = 'accessToken';
    const failMessage: string = 'Something went wrong! Please try again later!';
    const wrongRequest: string = 'Please, check your credentials!';

    const [validated, setValidated] = useState(false);
    const [infoMessage, setInfoMessage] = useState('');
    const [infoMessageClass, setInfoMessageClass] = useState('');

    const navigate = useNavigate();

    async function registerHandler() {
        setValidated(false);
        setInfoMessage('');
        setInfoMessageClass('');

        const emailValue: string | undefined = inputEmail.current?.value;
        const passwordValue: string | undefined = inputPassword.current?.value;

        if (validateForm()) {
            setValidated(true);

            try {
                const response = await axios.post(urlRegister, {
                    email: emailValue,
                    password: passwordValue,
                });

                if (response.status === 200) {
                    localStorage.setItem(
                        accessTokenKey,
                        'Bearer ' + response.data.accessToken
                    );
                    return navigate('/');
                }
            } catch (error) {
                if ((error as AxiosError).response?.status === 401) {
                    setInfoMessage(wrongRequest);
                    setInfoMessageClass('fail');
                } else {
                    setInfoMessage(failMessage);
                    setInfoMessageClass('fail');
                }
            }
        }

        function validateForm(): boolean {
            let result: boolean = true;

            if (
                emailValue === undefined ||
                emailValue.length < minEmailLength ||
                emailValue.length > maxEmailLength
            ) {
                result = false;
            }

            if (
                passwordValue === undefined ||
                passwordValue.length < minPasswordLength ||
                passwordValue.length > maxPasswordLength
            ) {
                result = false;
            }

            return result;
        }
    }

    return (
        <div>
            <h1>Login</h1>
            <Form ref={form} validated={validated}>
                <Form.Group className="mb-3" controlId="formBasicEmail">
                    <Form.Label>Email address</Form.Label>
                    <Form.Control
                        type="email"
                        placeholder="Enter your Email"
                        required
                        maxLength={maxEmailLength}
                        ref={inputEmail}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicPassword">
                    <Form.Label>Password</Form.Label>
                    <Form.Control
                        type="password"
                        placeholder="Enter your Password"
                        required
                        maxLength={maxPasswordLength}
                        ref={inputPassword}
                    />
                </Form.Group>

                <Button variant="primary" onClick={registerHandler}>
                    Submit
                </Button>
            </Form>
            <h4 className={infoMessageClass}>{infoMessage}</h4>
        </div>
    );
}
