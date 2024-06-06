import axios from "axios";
import { API_HOST } from "../consts";

export type candidateListItemType = {
    id: number,
    fio: string,
    dateOfBirth: Date,
    desiredPositionId: number,
    desiredPosition: string,
    gender: string,
    workExperience: number
}

class CandidateApi {
    getBearer() {
        return `Bearer ${localStorage.getItem('token')}`;
    }
    
    async add(firstName: string, lastName: string, patronymic: string, gender: number, educationType: number, seriePassport: string, numberPassport: string, desiredPositionId: number, dateOfBirth: Date, workExperience: number) : Promise<string>
    {
        let result = '';

        try {
            await axios.post(`${API_HOST}/api/candidate/add`, {
                firstName, lastName, patronymic,
                gender, educationType, seriePassport, numberPassport,
                dateOfBirth, desiredPositionId, workExperience
            }, {headers: {
                Authorization: this.getBearer()
            }})
            .then(res => result = '')
            .catch(err => {
                if (err.response.data)
                    result = err.response.data
            });
        }
        catch (ex) {

        }

        return result;
    }

    async get(id: number) : Promise< null> {
        let result : null = null;

        try {
            await axios.get(`${API_HOST}/api/employee/get?id=${id}`, {headers: {Authorization: this.getBearer()}})
                .then(res => {
                    if (res.data.firstName) {
                        
                    }
                })
                .catch(err => {});
        }
        catch(ex) {

        }

        return result;
    }

    async getPage(count: number, page: number, sortType: number, educationType: number, gender: number, departmentId: number) : Promise<candidateListItemType[]> {
        let result : candidateListItemType[] = [];

        try {
            await axios.get(`${API_HOST}/api/candidate/getpage?count=${count}&page=${page}&sortType=${sortType}&educationType=${educationType}&departmentId=${departmentId}&gender=${gender}`, {headers: {
                Authorization: this.getBearer()
            }})
            .then(res => {
                if (res.data) {
                    result = res.data;
                    console.log(res)
                }
            })
            .catch(err => {});
        }
        catch (ex) {
        }

        return result;
    }
}

export default new CandidateApi();