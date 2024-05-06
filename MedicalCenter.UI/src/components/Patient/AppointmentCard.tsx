import { Button, Col, Row } from 'react-bootstrap';
import { userAppointment } from '../../models/userAppointment';

interface Props {
    data: userAppointment;
    onCancel: () => void;
}

function AppointmentCard({ data, onCancel }: Props) {
    return (
        <Row
            xs={1}
            md={2}
            lg={2}
            style={{ border: 'none', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}
            className="align-items-center mb-3"
        >
            <Col>
                <h3>
                    {data.doctorInfo.firstName} {data.doctorInfo.lastName}
                </h3>
                <p>Specialization: {data.doctorInfo.specialization}</p>
                <p>
                    {new Date().getFullYear() -
                        data.doctorInfo.practiceStartDate}{' '}
                    years of practice
                </p>
            </Col>
            <Col className="align-items-center">
                <p>
                    Date: {data.date} {data.time}
                </p>
                {!data.isPast && (
                    <Button
                        variant="danger"
                        onClick={onCancel}
                        className="mb-3"
                    >
                        Cancel
                    </Button>
                )}
            </Col>
        </Row>
    );
}

export default AppointmentCard;
