import { makeAutoObservable } from "mobx";

export type UserStoreType = {
    id: number,
    login: string,
    imagePath: string,
    firstName: string,
    lastName: string,
    privilege: string,
    position: string
}

export default class UserStore {

    isAuth: boolean = false;
    user: UserStoreType | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    public setIsAuth(value: boolean) {
        this.isAuth = value;
        if (!value)
            this.user = null;
    }

    public setUser(value: UserStoreType | null) {
        this.user = value;
    }

}

