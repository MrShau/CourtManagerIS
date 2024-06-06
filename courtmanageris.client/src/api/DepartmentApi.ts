import axios from "axios"
import { API_HOST } from "../consts"

export type getAllNamesItem = {
    id: number,
    name: string
}

export type getPageDepartamentItem = {
    id: number,
    name: string,
    description: string,
    supervisor: string,
    positions: string,
    positionsCount: number,
    employeesCount: number
}

export type departmentType = {
    id: number,
    name: string,
    description: string,
    supervisorId: number,
    createdAt: Date
}

class DepartmentApi {
    getBearer() {
        return `Bearer ${localStorage.getItem('token')}`
    }

    async getAllNames() : Promise<getAllNamesItem[]> {
        let result : getAllNamesItem[] = []
        try {
            await axios.get(`${API_HOST}/api/department/getallnames`, {headers: {
                Authorization: this.getBearer()
            }})
            .then(res => {
                if (res.data) {
                    result = res.data;
                }
            })
            .catch(err => {});
        }
        catch(ex) {

        }
        return result;
    }

    async add(name: string, description: string) : Promise<string> {
        let result : string = "";

        try {
            await axios.post(`${API_HOST}/api/department/add`, {name, description }, { headers: { Authorization: this.getBearer()} })
                .then(res => {})
                .catch(res => result = res.response.data ?? '');
        } catch (ex) {}

        return result;
    }

    async getList(count: number, page: number) : Promise<getPageDepartamentItem[]> {
        let result: getPageDepartamentItem[] = []

        try {
            await axios.get(`${API_HOST}/api/department/getpage?count=${count}&page=${page}`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch(ex => {});
        } catch (ex) {}

        return result;
    }

    async get(id: number) : Promise<departmentType | null> {
        let result : departmentType | null = null;
        
        try {
            await axios.get(`${API_HOST}/api/department/get?id=${id}`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch(() => {});
        } catch (ex) {}

        return result;
    }

    async update(id: number, name: string | null, description: string | null) : Promise<string> {
        let result = "";

        try {
            await axios.patch(`${API_HOST}/api/department/update`, {id, name, description}, {headers: {Authorization: this.getBearer()}})
                .then()
                .catch(res => result = res.response.data);
        } catch(ex) {}

        return result;
    }


    async deleteSupervisor(id: number) : Promise<string> {
        let  result = "";
        try {
            await axios.patch(`${API_HOST}/api/department/deletesupervisor`, {id}, {headers: {Authorization: this.getBearer()}})
                .then()
                .catch(res => result = res.response.data);
        } catch (ex) {}
        return result;
    }

    async setSupervisor(id: number, supervisorLogin: string) : Promise<string> {
        let result = '';
        try {
            await axios.patch(`${API_HOST}/api/department/setsupervisor`, {id, supervisorLogin}, {headers: {Authorization: this.getBearer()}})
                .then(res => console.log(res))
                .catch(res => result = res.response.data);
        } catch (ex) {}
        return result;
    }
}

export default new DepartmentApi();