import axios from "axios";
import { API_HOST } from "../consts";
import { UserStoreType } from "../stores/UserStore";

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

export type UserType = {
    login: string,
    firstName: string,
    lastName: string,
    patronymic: string,
    seriePassport: string,
    numberPassport: string,
    gender: number,
    educationType: number,
    departmentId: number,
    positionId: number,
    dateOfBirth: Date,
    dateOfAppointment: Date
}

export type NotificationType = {
    id: number,
    title: string,
    content: string,
    senderId: number,
    senderFIO: string,
}

export type ChatMessageType = {
    id: number,
    login: string,
    message: string,
    dateTime: string
}

class EmployeeApi {
    hasToken() : boolean {
        return localStorage.getItem('token') != null;
    }

    getBearer() {
        return `Bearer ${localStorage.getItem('token')}`;
    }

    async signUp(login: string, password: string, firstName: string, lastName: string, patronymic: string, gender: number, educationType: number, seriePassport: string, numberPassport: string, departmentId: number, positionId: number, dateOfBirth: Date) : Promise<string>
    {
        let result = '';

        try {
            await axios.post(`${API_HOST}/api/employee/signup`, {
                login, password, firstName, lastName, patronymic,
                gender, educationType, seriePassport, numberPassport,
                departmentId, positionId, dateOfBirth
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

    async signIn(login: string, password: string) : Promise<string> {
        let result : string = '';
        try {
            await axios.post(`${API_HOST}/api/employee/signin`, {login, password})
                .then(res => {
                    if (res.data)
                    {
                        localStorage.setItem('token', res.data)
                        window.location.hash = '#/';
                        window.location.reload();
                    }
                })
                .catch(err => {
                    if (err.response.data) 
                        result = err.response.data;
                });
        }
        catch (ex) {
            result = 'Ошибка сервера';
        }
        return result;
    }
    
    async updateBaseInfo(
        employeeId: number, 
        login: string | null, 
        firstName: string | null, 
        lastName: string | null, 
        patronymic: string | null, 
        gender: number | null, 
        seriePassport: string | null, 
        numberPassport: string | null, 
        educationType: number | null, 
        editDateOfBirth: Date | null
    ) : Promise<string> {
        let result : string = "";
        try {
            if (editDateOfBirth == null || editDateOfBirth?.getFullYear() > 2006 || editDateOfBirth?.getFullYear() < 1920)
                editDateOfBirth = null;
            await axios.patch(`${API_HOST}/api/employee/update`, {
                id: employeeId,
                login,
                firstName,
                lastName,
                patronymic,
                gender,
                seriePassport,
                numberPassport,
                educationType,
                editDateOfBirth
            }, {headers: {Authorization: this.getBearer()}})
            .then()
            .catch(err => err.response?.data ?? '');
        } catch (ex) {
            result = "Ошибка сервера";
            console.log(ex)
        }
        return result;
    }

    async updateEmployeeDepartment(employeeId: number, positionId: number) : Promise<string> {
        let result : string = '';
        try {
            await axios.patch(`${API_HOST}/api/employee/updatedepartment`, {
                id: employeeId, positionId
            }, { headers: {
                Authorization: this.getBearer()
            }})
            .then(() => alert('Изменения применены'))
            .catch(res => {
                result = res.response?.data ?? 'Ошибка';
                console.log(res)
            })
        }
        catch (ex) {
            result = "Ошибка сервера";
        }
        return result;
    }

    async updateEmployeePosition(employeeId: number, positionId: number) : Promise<string> {
        let result : string = '';
        try {
            await axios.patch(`${API_HOST}/api/employee/updatedepartment`, {
                id: employeeId, positionId
            }, { headers: {
                Authorization: this.getBearer()
            }})
            .then(() => alert('Изменения применены'))
            .catch(res => {
                result = res.response?.data ?? 'Ошибка';
                console.log(res)
            })
        }
        catch (ex) {
            result = "Ошибка сервера";
        }
        return result;
    }

    async updatePassword(employeeId: number, password: string) {
        try {
            await axios.patch(`${API_HOST}/api/employee/changepassword`, {
                id: employeeId, 
                password
            }, {
                headers : {
                    Authorization: this.getBearer()
                }
            })
            .then()
            .catch();
        }
        catch (ex) {

        }
    }

    async auth() : Promise<UserStoreType | null>{
        let result : UserStoreType | null = null;
        if (!this.hasToken())
            return result;
        try {
            await axios.get(`${API_HOST}/api/employee/auth`, {
                headers: {
                    Authorization: this.getBearer()
                }
            })
            .then(res => {
                if (res.data && res.data.login)
                    result = res.data;
            })
            .catch(ex => {})
        }
        catch(ex) {

        }
        return result;
    }

    async block(id: number, value: boolean) {
        try {
            await axios.patch(`${API_HOST}/api/employee/block`, {id, value}, {headers: {Authorization: this.getBearer()}})
                .then(() => alert(value ? 'Пользователь заблокирован' : 'Пользователь разблокирован'))
                .catch();
        }
        catch (ex) {
            console.log(ex)
        }
    }

    async getPage(count: number, page: number, sortType: number, educationType: number, gender: number, departmentId: number, loginLike: string) : Promise<getPageItemType[]> {
        let result : getPageItemType[] = [];

        try {
            await axios.get(`${API_HOST}/api/employee/getpage?count=${count}&page=${page}&sortType=${sortType}&loginLike=${loginLike}&educationType=${educationType}&departmentId=${departmentId}&gender=${gender}`, {headers: {
                Authorization: this.getBearer()
            }})
            .then(res => {
                if (res.data) {
                    result = res.data;
                }
            })
            .catch(err => {});
        }
        catch (ex) {
        }

        return result;
    }

    async get(id: number) : Promise<UserType | null> {
        let result : UserType | null = null;

        try {
            await axios.get(`${API_HOST}/api/employee/get?id=${id}`, {headers: {Authorization: this.getBearer()}})
                .then(res => {
                    if (res.data.firstName) {
                        result = res.data
                    }
                })
                .catch(err => {});
        }
        catch(ex) {

        }

        return result;
    }

    async activityEnd() {
        try {
            await axios.post(`${API_HOST}/api/employee/activityend`, {}, {headers: {Authorization: this.getBearer()}})
            .then()
            .catch();
        }
        catch (ex) {}
    }

    async sendNotification(recipientId: number, title: string, content: string) {
        try {
            await axios.post(`${API_HOST}/api/notification/send`, {recipientId, title, content}, {headers: {Authorization: this.getBearer()}}).then().catch();
        } catch(ex) {}
    }

    async getNotifications() : Promise<NotificationType[]> {
        let result : NotificationType[] = [];
        try {
            await axios.get(`${API_HOST}/api/notification/get`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch(ex) {}
        return result;
    }

    async getChatMessages() : Promise<ChatMessageType[]> {
        let result : ChatMessageType[] = [];
        try {
            await axios.get(`${API_HOST}/api/chat/getmessages`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch();
        } catch(ex) {

        }
        return result;
    }

    async getChart() : Promise<number[]> {
        let result: number[] = [];

        try {
            await axios.get(`${API_HOST}/api/employee/getchart`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch (ex) {}

        return result;
    }

    async getCount() : Promise<number> {
        let result = 0;

        try {
            await axios.get(`${API_HOST}/api/employee/getcount`, {headers: {Authorization: this.getBearer()}})
                .then(res => result = res.data)
                .catch()
        } catch (ex) {}

        return result;
    }
}

export default new EmployeeApi();