export const API_HOST = 'http://courtmanager.somee.com';

export function checkAuth() : void {
    if (localStorage.getItem('token') == null) {
        window.location.hash = "#/auth";
    }
}

export function dateToString(d: Date) : string {
    let date = new Date(d);
    return `${date.toLocaleDateString()}`
}

export function toFormattedDate(date: Date) : string {
    var dd = date.getDate().toString();
    if (date.getDate() < 10) dd = '0' + dd;
    var mm = (date.getMonth() + 1).toString();
    if (date.getMonth() + 1 < 10) mm = '0' + mm;
    var yyyy = date.getFullYear();
    /* change format here */
    return String(yyyy + '-' + mm + '-' + dd);
  };