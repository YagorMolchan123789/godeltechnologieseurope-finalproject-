import axios, { AxiosInstance, AxiosResponse } from 'axios';
import { getAllDoctorsDto } from '../models/getAllDoctorsDto';
import { userAppointment } from '../models/userAppointment';
const API_URL = process.env.REACT_APP_API_URL;

const getAccessToken = (): string | null => {
    return localStorage.getItem('accessToken');
};

const api = (): AxiosInstance =>
    axios.create({
        headers: {
            Authorization: getAccessToken(),
            'Access-Control-Allow-Headers': '*',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': '*',
            Allow: '*',
        },
    });

const apiConnector = {
    getAllDoctors: async (): Promise<getAllDoctorsDto> => {
        try {
            const url = API_URL + 'api/doctors';
            const response: AxiosResponse<getAllDoctorsDto> =
                await api().get(url);

            return response.data as getAllDoctorsDto;
        } catch (error: unknown) {
            console.error('Failed to fetch doctors:', error);
            return {
                doctorInfos: [],
                isShowButton: false,
            };
        }
    },

    deleteDoctor: async (doctorId: string): Promise<void> => {
        const url = API_URL + `api/doctors/${doctorId}`;
        await api().delete<string>(url);
    },
    getUserAppointments: async (): Promise<userAppointment[]> => {
        try {
            const { data } = await api().get<userAppointment[]>(
                API_URL + 'api/appointments'
            );
            return data;
        } catch (error: unknown) {
            console.error('Failed to get appointments:', error);
            return [];
        }
    },
    cancelAppointment: async (id: number) => {
        const url = API_URL + `api/appointments/${id}`;
        await api().delete(url);
    },
};

export default apiConnector;
