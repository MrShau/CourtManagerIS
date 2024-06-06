import React from "react";
import { Col, Container, Row } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";

function CourtCases() {
    return (
        <>
        <Row className='h-100 m-0'>
            <Col className="background-dark col-4 col-lg-3 col-xxl-2 p-0 m-0 vh-100"><Sidebar activePage="/courtcases"/></Col>
            <Col className='background-dark col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                <main className='background-light p-2 p-lg-3 p-xl-4 rounded-2 h-100 w-100 overflow-auto'>
                    <Container className="p-3" fluid>
                    </Container>
                </main>
            </Col>
        </Row>
        </>
    )
}

export default CourtCases