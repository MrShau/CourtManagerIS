import axios from "axios";
import { API_HOST } from "../consts";

export type getAllNamesItem = {
    id: number,
    name: string
}

class PositionApi {
    getBearer() : string {
        return `Bearer ${localStorage.getItem('token')}`
    }

    async getAllNames(departmentId: number) : Promise<getAllNamesItem[]> {
        let result : getAllNamesItem[] = [];
        try {
            await axios.get(`${API_HOST}/api/position/getallnames?departmentId=${departmentId}`, {
                headers : {
                    Authorization: this.getBearer()
                }
            })
            .then(res => {
                if (res.data)
                    result = res.data;
            })
            .catch(err => {})
        }
        catch(ex) {

        }
        return result;
    }
}

export default new PositionApi();