import axios from 'axios';
import { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Alert from 'react-bootstrap/Alert';
import { toast } from 'react-toastify';
import { useLocation, useParams, useNavigate } from 'react-router-dom';
import { Col } from 'react-bootstrap';
import DoctorImage from '../doctors/DoctorImage';

interface TimeSlot {
    id: number;
    startTime: string;
    endTime: string;
}

interface TimeSlotsData {
    isLoading: boolean;
    currentTime: string;
    timeSlots: TimeSlot[];
}

interface GetTimeSlotsRequest {
    doctorId: string;
    date: string;
}

const API_URL = process.env.REACT_APP_API_URL;

export const AppointmentTimeForm = () => {
    const params = useParams();
    const doctorId = params.doctorId;
    const accessToken = localStorage.getItem('accessToken');

    const location = useLocation();
    const { doctor } = location.state;

    const [timeSlotsData, setTimeSlotsData] = useState<TimeSlotsData>({
        isLoading: true,
        currentTime: '',
        timeSlots: [],
    });

    const [date, setDate] = useState<string>('');
    const [selectedTimeSlot, setSelectedTimeSlot] = useState<number>(0);

    const headers = {
        Authorization: accessToken,
        'Access-Control-Allow-Headers': '*',
        'Access-Control-Allow-Origin': '*',
        'Access-Control-Allow-Methods': '*',
        Allow: '*',
    };
    const navigate = useNavigate();

    const loadTimeSlots = async (e: React.ChangeEvent<HTMLDataElement>) => {
        const selectedDate = e.target.value;

        setDate(selectedDate);
        setSelectedTimeSlot(0);
        setTimeSlotsData({
            isLoading: true,
            currentTime: '',
            timeSlots: [],
        });

        if (!selectedDate) {
            return;
        }

        const url = API_URL + 'api/timeslot/available?api-version=1.0';

        const requestParams: GetTimeSlotsRequest = {
            doctorId: doctorId ?? '',
            date: selectedDate,
        };

        const api = axios.create({
            baseURL: url,
            headers: headers,
        });

        try {
            const { data } = await api.get<TimeSlotsData>(url, {
                params: requestParams,
            });
            setTimeSlotsData({
                isLoading: false,
                currentTime: removeSeconds(data.currentTime),
                timeSlots: data.timeSlots.map((t) => {
                    return {
                        id: t.id,
                        startTime: removeSeconds(t.startTime),
                        endTime: removeSeconds(t.endTime),
                    };
                }),
            });
        } catch (error) {
            console.error(error);
        }
    };

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        await createAppointment();

        setDate('');
        setTimeSlotsData({
            isLoading: true,
            currentTime: '',
            timeSlots: [],
        });
    };

    const createAppointment = async () => {
        const url = API_URL + 'api/appointment';
        try {
            await axios.post(
                url,
                {
                    doctorId: doctorId,
                    date: date,
                    timeSlotId: selectedTimeSlot,
                },
                {
                    headers: headers,
                }
            );

            toast.success(
                <div>
                    Appointment created <br />
                    Date: {date} <br />
                    Time:{' '}
                    {
                        timeSlotsData.timeSlots.find(
                            (t) => t.id == selectedTimeSlot
                        )?.startTime
                    }
                    <br />
                </div>,
                {
                    position: 'bottom-center',
                    autoClose: 6000,
                }
            );
            navigate('/user/appointments');
        } catch (error) {
            console.error(error);
        }
    };

    const removeSeconds = (timeString: string): string => {
        const [hours, minutes] = timeString.split(':');
        return `${hours}:${minutes}`;
    };

    const timeSelectInput = () => {
        if (!timeSlotsData.isLoading && timeSlotsData.timeSlots.length == 0) {
            return (
                <Alert variant="danger">
                    No available appointments for selected date!
                </Alert>
            );
        }

        if (!timeSlotsData.isLoading) {
            return (
                <Form.Group className="mb-3" controlId="selectDate">
                    <Form.Label className="mb-3">
                        Current time: {timeSlotsData.currentTime}
                    </Form.Label>
                    <Form.Select
                        aria-label="Select time for appointment"
                        name="timeSlotId"
                        onChange={(e) =>
                            setSelectedTimeSlot(Number(e.target.value))
                        }
                    >
                        <option>{'Select time for appointment'}</option>
                        {timeSlotsData.timeSlots.map((timeSlot: TimeSlot) => (
                            <option key={timeSlot.id} value={timeSlot.id}>
                                {`${timeSlot.startTime} - ${timeSlot.endTime}`}
                            </option>
                        ))}
                    </Form.Select>
                </Form.Group>
            );
        }
    };

    return (
        <>
            <Row className="justify-content-center align-items-center mt-3">
                <Col xs={10} sm={6} md={3} lg={4}>
                    <DoctorImage
                        imageNameWithType={doctor.photoUrl}
                        style={{ maxWidth: '100%', height: 'auto' }}
                    />
                </Col>
                <Col xs={12} sm={6} md={3} lg={4}>
                    <h1>{`${doctor.firstName} ${doctor.lastName}`}</h1>
                    Specialization: {doctor.specialization}
                    <hr />
                    {new Date().getFullYear() - doctor.practiceStartDate} years
                    of practice
                    <hr />
                    <Form className="mx-auto" onSubmit={handleSubmit}>
                        <Form.Group className="mb-3" controlId="selectDate">
                            <Form.Label>Date of visit</Form.Label>
                            <Form.Control
                                type="date"
                                min={new Date().toISOString().split('T')[0]}
                                placeholder="Select date"
                                value={date}
                                onChange={loadTimeSlots}
                            />
                        </Form.Group>
                        {date && timeSelectInput()}
                        <Button
                            className="mb-3"
                            type="submit"
                            variant={
                                date && selectedTimeSlot
                                    ? 'primary'
                                    : 'secondary'
                            }
                            disabled={date && selectedTimeSlot ? false : true}
                        >
                            Create Appointment
                        </Button>
                    </Form>
                </Col>
            </Row>
        </>
    );
};
