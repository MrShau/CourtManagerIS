import axios from "axios";
import { API_HOST } from "../consts";
export type getPageItemType = {
    id: number,
    login: string,
    firstName: string,
    lastName: string,
    patronymic: string,
    gender: string,
    dateOfBirth: Date,
    department: string,
    position: string,
    isBlocked: boolean
}

class JudgeApi {

    getBearer() {
        return `Bearer ${localStorage.getItem('token')}`;
    }

    async add(login: string, password: string, firstName: string, lastName: string, patronymic: string, gender: number, educationType: number, seriePassport: string, numberPassport: string, dateOfBirth: Date) : Promise<string>
    {
        let result = '';

        try {
            await axios.post(`${API_HOST}/api/judge/add`, {
                login, password, firstName, lastName, patronymic,
                gender, educationType, seriePassport, numberPassport,
                dateOfBirth
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

    async getList() : Promise<getPageItemType[]> {
        let result : getPageItemType[] = [];
        try {
            await axios.get(`${API_HOST}/api/judge/getpage`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch (ex) {}

        return result;
    }

}

export default new JudgeApi()