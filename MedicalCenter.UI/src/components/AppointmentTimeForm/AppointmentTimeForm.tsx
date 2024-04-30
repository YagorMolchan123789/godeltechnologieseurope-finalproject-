import axios from 'axios';
import { useState } from 'react';
import Form from 'react-bootstrap/Form';
import Button from 'react-bootstrap/Button';
import Row from 'react-bootstrap/Row';
import Alert from 'react-bootstrap/Alert';
import { toast } from 'react-toastify';
import { useParams } from 'react-router-dom';

interface TimeSlot {
    id: number;
    startTime: string;
    endTime: string;
}

interface TimeSlotsData {
    isLoading: boolean;
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

    const [timeSlotsData, setTimeSlotsData] = useState<TimeSlotsData>({
        isLoading: true,
        timeSlots: [],
    });

    const [date, setDate] = useState<string>('');
    const [selectedTimeSlot, setSelectedTimeSlot] = useState<number>(0);

    const loadTimeSlots = async (e: React.ChangeEvent<HTMLDataElement>) => {
        const selectedDate = e.target.value;

        setDate(selectedDate);
        setSelectedTimeSlot(0);
        setTimeSlotsData({
            isLoading: true,
            timeSlots: [],
        });

        if (!selectedDate) {
            return;
        }

        const url = API_URL + 'api/timeslot/available';

        const requestParams: GetTimeSlotsRequest = {
            doctorId: doctorId ?? '',
            date: selectedDate,
        };

        const api = axios.create({
            baseURL: url,
            headers: {},
        });

        try {
            const { data } = await api.get<TimeSlot[]>(url, {
                params: requestParams,
            });
            setTimeSlotsData({
                isLoading: false,
                timeSlots: data.map((t) => {
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
            timeSlots: [],
        });
    };

    const createAppointment = async () => {
        const url = API_URL + 'api/appointment';
        try {
            await axios.post(url, {
                doctorId: doctorId,
                date: date,
                timeSlotId: selectedTimeSlot,
            });

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
                <Form.Select
                    className="mb-3"
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
            );
        }
    };

    return (
        <Row>
            <Form className="col-md-3 mx-auto" onSubmit={handleSubmit}>
                <Form.Group
                    className="mb-3"
                    controlId="exampleForm.ControlInput1"
                >
                    <Form.Label>Date of visit</Form.Label>
                    <Form.Control
                        type="date"
                        placeholder="Select date"
                        value={date}
                        onChange={loadTimeSlots}
                    />
                </Form.Group>
                {date && timeSelectInput()}
                <Button
                    className="mb-3"
                    type="submit"
                    variant={date && selectedTimeSlot ? 'primary' : 'secondary'}
                    disabled={date && selectedTimeSlot ? false : true}
                >
                    Create Appointment
                </Button>
            </Form>
        </Row>
    );
};
