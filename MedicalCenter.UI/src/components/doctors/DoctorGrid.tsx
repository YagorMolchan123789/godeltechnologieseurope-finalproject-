import { DoctorDto } from '../../models/doctorDto';
import React, { useState, useEffect } from 'react';
import Container from 'react-bootstrap/Container';
import apiConnector from '../../api/apiconnector';
import { Col, Row } from 'react-bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css';
import DoctorCard from './DoctorCard';

export default function DoctorGrid() {
    const [doctors, setDoctors] = useState<DoctorDto[]>([]);

    useEffect(() => {
        const fetchData = async () => {
            const fetchedDoctors = await apiConnector.getAllDoctors();
            setDoctors(fetchedDoctors);
        };

        fetchData();
    }, []);

    return (
        <Container>
            <h1 className="doctors-page-title">Our doctors</h1>
            <Row
                xs={1}
                md={2}
                lg={3}
                className="justify-content-center doctor-row-container"
            >
                {doctors.length !== 0 &&
                    doctors.map((doctor, index) => (
                        <Col key={doctor.appUserId} className="doctor-col">
                            <DoctorCard key={index} doctor={doctor} />
                        </Col>
                    ))}
            </Row>
        </Container>
    );
}
