import { useRef, useState } from 'react';
import './Register.css';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios, { AxiosError } from 'axios';

export default function Register() {
    const minEmailLength: number = 5;
    const maxEmailLength: number = 256;
    const minPasswordLength: number = 8;
    const maxPasswordLength: number = 20;
    const minNameLength: number = 1;
    const maxNameLength: number = 20;
    const urlRegister: string = process.env.REACT_APP_API_URL + 'api/register';

    const inputEmail = useRef<HTMLInputElement>(null);
    const inputPassword = useRef<HTMLInputElement>(null);
    const inputFirstName = useRef<HTMLInputElement>(null);
    const inputLastName = useRef<HTMLInputElement>(null);
    const form = useRef<HTMLFormElement>(null);

    const successMessage: string =
        'You successfully registered and can log in now!';
    const wrongRequest: string =
        'It is similar to that a user with this email already exists!';
    const failMessage: string = 'Something went wrong! Please try again later!';

    const [validated, setValidated] = useState(false);
    const [infoMessage, setInfoMessage] = useState('');
    const [infoMessageClass, setInfoMessageClass] = useState('');

    async function registerHandler() {
        setValidated(false);
        setInfoMessage('');
        setInfoMessageClass('');

        const emailValue: string | undefined = inputEmail.current?.value;
        const passwordValue: string | undefined = inputPassword.current?.value;
        const firstNameValue: string | undefined =
            inputFirstName.current?.value;
        const lastNameValue: string | undefined = inputLastName.current?.value;

        if (validateForm()) {
            setValidated(true);

            try {
                const response = await axios.post(urlRegister, {
                    email: emailValue,
                    password: passwordValue,
                    firstName: firstNameValue,
                    lastName: lastNameValue,
                });

                if (response.status === 201) {
                    setInfoMessage(successMessage);
                    setInfoMessageClass('success');
                }
            } catch (error) {
                if ((error as AxiosError).response?.status === 400) {
                    setInfoMessage(wrongRequest);
                    setInfoMessageClass('fail');
                    return;
                }

                setInfoMessage(failMessage);
                setInfoMessageClass('fail');
            }
        }

        function validateForm(): boolean {
            if (
                emailValue === undefined ||
                emailValue.length < minEmailLength ||
                emailValue.length > maxEmailLength
            )
                return false;

            if (
                passwordValue === undefined ||
                passwordValue.length < minPasswordLength ||
                passwordValue.length > maxPasswordLength
            )
                return false;

            if (
                firstNameValue === undefined ||
                firstNameValue.length < minNameLength ||
                firstNameValue.length > maxNameLength
            )
                return false;

            if (
                lastNameValue === undefined ||
                lastNameValue.length < minNameLength ||
                lastNameValue.length > maxNameLength
            )
                return false;

            return true;
        }
    }

    return (
        <div>
            <h1>Register</h1>
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
                        placeholder="min. 8 chars, required one big and one small letter, number, special char"
                        required
                        maxLength={maxPasswordLength}
                        ref={inputPassword}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicFirstName">
                    <Form.Label>First Name</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter your First Name"
                        required
                        maxLength={maxNameLength}
                        ref={inputFirstName}
                    />
                </Form.Group>

                <Form.Group className="mb-3" controlId="formBasicLastName">
                    <Form.Label>Last Name</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter your Last Name"
                        required
                        maxLength={maxNameLength}
                        ref={inputLastName}
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
