export interface userAppointment {
    id: number;
    time: string;
    date: string;
    doctorInfo: {
        firstName: string;
        lastName: string;
        specialization: string;
        practiceStartDate: number;
    };
    isPast: boolean;
};