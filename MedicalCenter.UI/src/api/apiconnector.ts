import { DoctorDto } from '../models/doctorDto';
import axios, { AxiosResponse } from 'axios';
const API_URL = process.env.REACT_APP_API_URL;

const apiConnector = {
    getAllDoctors: async (): Promise<DoctorDto[]> => {
        try {
            const accessToken = localStorage.getItem('accessToken');
            const url = API_URL + 'api/doctors';

            const api = axios.create({
                headers: {
                    Authorization: accessToken,
                    'Access-Control-Allow-Headers': '*',
                    'Access-Control-Allow-Origin': '*',
                    'Access-Control-Allow-Methods': '*',
                    Allow: '*',
                },
            });

            const response: AxiosResponse<DoctorDto[]> = await api.get(url);

            const data: DoctorDto[] = await response.data;

            return data;
        } catch (error: unknown) {
            console.error('Failed to fetch doctors:', error);
            return [];
        }
    },
};

export default apiConnector;
