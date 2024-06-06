import axios from "axios";
import { API_HOST } from "../consts";

export type EventType = {
    id: number,
    title: string,
    description: string,
    startDate: string,
    endDate: string,
    createrFIO: string
}

class EventApi {
    getBearer() {
        return `Bearer ${localStorage.getItem('token')}`;
    }

    async add(title: string, description: string, startDate: Date | null, endDate: Date | null) {
        try {
            await axios.post(`${API_HOST}/api/event/add`, {title, description, startDate, endDate}, {headers: {Authorization: this.getBearer()}})
                .then(() => window.location.reload())
                .catch(res => {if (res.response.data !== null) alert(res.response.data)});
        } catch (ex){}
    }

    async getActives(count: number = 0) : Promise<EventType[]> {
        let result : EventType[] = [];
        try {
            await axios.get(`${API_HOST}/api/event/getactives?count=${count}`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch(ex) {}
        return result;
    }

    async getCompleted(count: number = 0) : Promise<EventType[]> {
        let result : EventType[] = [];
        try {
            await axios.get(`${API_HOST}/api/event/getcompleted?count=${count}`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch(ex) {}
        return result;
    }
}

export default new EventApi();