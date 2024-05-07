import { useRef, useState } from 'react';
import './RegisterDoctor.css';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import axios, { AxiosError } from 'axios';

export default function RegisterDoctor() {
    const minEmailLength: number = 5;
    const maxEmailLength: number = 256;
    const minPasswordLength: number = 8;
    const maxPasswordLength: number = 20;
    const minNameLength: number = 1;
    const maxNameLength: number = 20;
    const minSpecializationLength: number = 1;
    const maxSpecializationLength: number = 50;
    const urlRegisterDoctor: string =
        process.env.REACT_APP_API_URL + 'api/doctors';

    const date: Date = new Date();

    const inputEmail = useRef<HTMLInputElement>(null);
    const inputPassword = useRef<HTMLInputElement>(null);
    const inputFirstName = useRef<HTMLInputElement>(null);
    const inputLastName = useRef<HTMLInputElement>(null);
    const inputPracticeStartDate = useRef<HTMLInputElement>(null);
    const inputSpecialization = useRef<HTMLInputElement>(null);
    const form = useRef<HTMLFormElement>(null);

    const successMessage: string = 'You successfully register a doctor!';
    const wrongRequest: string =
        'It is similar to that a doctor with this email already exists!';
    const failMessage: string = 'Something went wrong! Please try again later!';
    const wrongCredentials: string = 'Please, check your credentials!';

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
        const practiceStartDateValue: string | undefined =
            inputPracticeStartDate.current?.value;
        const specializationValue: string | undefined =
            inputSpecialization.current?.value;

        if (validateForm()) {
            setValidated(true);

            try {
                const accessToken = localStorage.getItem('accessToken');

                const api = axios.create({
                    headers: {
                        Authorization: accessToken,
                        'Access-Control-Allow-Headers': '*',
                        'Access-Control-Allow-Origin': '*',
                        'Access-Control-Allow-Methods': '*',
                        Allow: '*',
                    },
                });

                const response = await api.post(urlRegisterDoctor, {
                    email: emailValue,
                    password: passwordValue,
                    firstName: firstNameValue,
                    lastName: lastNameValue,
                    practiceStartDate: +practiceStartDateValue!,
                    specialization: specializationValue,
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
        } else {
            setInfoMessage(wrongCredentials);
            setInfoMessageClass('fail');
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

            if (
                practiceStartDateValue === undefined ||
                +practiceStartDateValue > date.getFullYear()
            )
                return false;

            if (
                specializationValue === undefined ||
                specializationValue.length < minSpecializationLength ||
                specializationValue.length > maxSpecializationLength
            )
                return false;

            return true;
        }
    }

    return (
        <div>
            <h1>Register doctor</h1>
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

                <Form.Group
                    className="mb-3"
                    controlId="formBasicPracticeStartDate"
                >
                    <Form.Label>Practice Start Date</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter doctor practice start year (e.x. 1989)"
                        required
                        ref={inputPracticeStartDate}
                    />
                </Form.Group>

                <Form.Group
                    className="mb-3"
                    controlId="formBasicSpecialization"
                >
                    <Form.Label>Specialization</Form.Label>
                    <Form.Control
                        type="text"
                        placeholder="Enter doctor specialization"
                        required
                        maxLength={maxSpecializationLength}
                        ref={inputSpecialization}
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
