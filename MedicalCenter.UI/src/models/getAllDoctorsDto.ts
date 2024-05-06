import { DoctorDto } from './doctorDto';

export interface getAllDoctorsDto {
    doctorInfos: DoctorDto[];
    isShowButton: boolean;
}
