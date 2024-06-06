import React, { useContext, useEffect, useState } from "react";
import { Button, Container, Navbar } from "react-bootstrap";
import './Sidebar.css'
import { NavLink } from "react-router-dom";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faBell, faBriefcase, faBuildingUser, faCalendarDays, faGavel, faHome, faMessage, faPaperPlane, faPhone, faSignOutAlt, faUserCheck, faUsers } from '@fortawesome/free-solid-svg-icons'
import { Context } from "../..";
import { API_HOST } from "../../consts";


function Sidebar(props: { activePage: string | null }) {

    const navs = [
        {
            name: "Главная",
            route: "/",
            icon: faHome
        },
        {
            name: "Сотрудники",
            route: "/employees",
            icon: faUsers
        },
        {
            name: "Судьи",
            route: "/judges",
            icon: faGavel
        },
        {
            name: "Кандидаты",
            route: '/candidates',
            icon: faUserCheck
        },
        {
            name: "Отделы",
            route: '/departments',
            icon: faBuildingUser
        },
        {
            name: "Судебные дела",
            route: '/courtcases',
            icon: faBriefcase 
        },
        {
            name: "Мероприятия",
            route: "/events",
            icon: faCalendarDays
        },
        {
            name: "Уведомления",
            route: "/notifications",
            icon: faBell
        },
        {
            name: "Общий чат",
            route: "/chat",
            icon: faMessage
        }];

    const [stores, setStores] = useState(useContext(Context));
    

    return (
        <div className="sidebar background-dark ps-3 pe-0">
            <div className="sidebar-header user-profile-block pt-4 pb-3 overflow-hidden">
                <div className="user-image-block overflow-hidden mx-auto rounded-5">
                    <img
                        src={`${API_HOST}${stores?.user.user?.imagePath}`}
                        width={"100%"}
                        height={"100%"}
                        className="object-fit-cover"
                    />
                </div>
                <div className="sidebar-content user-info-block text-center mt-3">
                    <span className="d-block">{stores?.user.user?.firstName} {stores?.user.user?.lastName}</span>
                    <span className="text-secondary small d-block">{stores?.user.user?.position}</span>
                </div>
                <div className="w-100 background-light mt-4 " style={{ height: '3px' }}></div>
            </div>
            <div className="pt-4 overflow-y-auto  mb-4">
                {navs.map((nav, index) => <NavLink to={nav.route} key={index}><Button variant={nav.route === props.activePage ?? '' ? 'light text-black' : 'outline-dark'} className="w-100 text-light py-2 mb-1 rounded-0 border-0"><div className="d-flex"><div><FontAwesomeIcon icon={nav.icon} fixedWidth /></div> <div className="ps-3">{nav.name}</div></div></Button></NavLink>)}
                <Button variant='outline-dark' className="w-100 text-light py-2 mb-1 rounded-0 border-0" onClick={() => {
                    localStorage.removeItem('token');
                    window.location.reload()
                }}><div className="d-flex"><div><FontAwesomeIcon icon={faSignOutAlt} fixedWidth /></div> <div className="ps-3">Выйти</div></div></Button>
            </div>
        </div>
    )

}

export default Sidebar;