import React from 'react';
import { Card, Button } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import { DoctorDto } from '../../models/doctorDto';
import DoctorImage from './DoctorImage';
import { Link } from 'react-router-dom';

interface Props {
    doctor: DoctorDto;
}

function DoctorCard({ doctor }: Props) {
    return (
        <Card
            className="doctor-card"
            style={{ border: 'none', boxShadow: '0 4px 8px rgba(0,0,0,0.1)' }}
        >
            <DoctorImage
                imageNameWithType={doctor.photoUrl}
                style={{ maxWidth: '100%', height: 'auto' }}
            />
            <Card.Body className="doctor-card-body">
                <Card.Title className="doctor-card-title">
                    {doctor.firstName} {doctor.lastName}
                </Card.Title>
                <Card.Text>Specialization: {doctor.specialization}</Card.Text>
                <hr />
                <Card.Text>
                    {new Date().getFullYear() - doctor.practiceStartDate} years
                    of practice
                </Card.Text>
                <Link
                    state={{ doctor: doctor }}
                    to={'/../appointment/create/' + doctor.appUserId}
                >
                    <Button variant="primary">Choose time</Button>
                </Link>
            </Card.Body>
        </Card>
    );
}

export default DoctorCard;
