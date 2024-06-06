import React, { createRef, useContext, useEffect, useState } from "react";
import { Button, Col, Form, Row } from "react-bootstrap";
import Sidebar from "../../components/Sidebar/Sidebar";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { API_HOST } from "../../consts";
import EmployeeApi, { ChatMessageType } from "../../api/EmployeeApi";
import { Context } from "../..";


function Chat() {
    let [connection, setConnection] = useState<HubConnection | undefined>(
        undefined
    );

    const [message, setMessage] = useState<string>("");
    const [messages, setMessages] = useState<ChatMessageType[]>([]);

    const [stores, setStores] = useState(useContext(Context));

    const inputRef = createRef<HTMLTextAreaElement>();

    useEffect(() => {

        connection = new HubConnectionBuilder()
            .withUrl(`${API_HOST}/chat`)
            .build();

        try {
            connection.start().then(() => setConnection(connection)).catch(ex => console.log(ex));
            connection.onclose(() => setConnection(undefined));
            connection.onreconnecting(() => setConnection(undefined))

            connection.on("Receive", () => {
                EmployeeApi.getChatMessages().then(res => {setMessages(res); document.getElementById('chat-block')?.scrollTo(0, document.getElementById('chat-block')?.scrollHeight ?? 0)});
                
    ;


            })
        } catch (ex) { }
        EmployeeApi.getChatMessages().then(res => {setMessages(res); document.getElementById('chat-block')?.scrollTo(0, document.getElementById('chat-block')?.scrollHeight ?? 0)});
    }, []);
    return (
        <>
            <Row className='h-100 m-0'>
                <Col className="background-dark col-4 col-lg-3 col-xxl-2 p-0 m-0 vh-100"><Sidebar activePage="/chat" /></Col>
                <Col className='background-dark col-8 col-lg-9 col-xxl-10 py-2 pe-2 py-md-3 pe-md-3 py-lg-4 pe-lg-4 m-0 p-0'>
                    <main className='background-light p-3 p-xl-4  rounded-2 h-100 w-100 overflow-hidden' style={{
                        display: 'flex',
                        flexFlow: 'column'
                    }}>
                        <div className="overflow-auto" style={{ flex: "1 1 0px" }}  id='chat-block'>
                            {messages.map((item, index) => {
                                return (<div key={index} className={`mb-2 d-flex w-100`} style={{justifyContent: stores?.user.user?.login === item.login ? 'end' : 'start'}}>
                                    <div className="rounded-2 border p-2 bg-body" style={{display: 'inline-block', verticalAlign: 'top'}}>
                                    <div style={{overflowWrap: 'break-word', wordWrap: 'break-word', wordBreak: 'break-all'}}><span className="text-secondary small " >#{item.login}:</span> {item.message}</div>
                                    <div className="d-flex ms-2 text-secondary align-items-center justify-content-end">
                                        <div style={{fontSize: '12px'}}>{item.dateTime}</div>
                                    </div>
                                    </div>
                                </div>)
                            })}
                        </div>
                        <div className="d-flex">
                            <Form.Control
                                className="w-100 py-2 rounded-0"
                                as="textarea"
                                rows={1}
                                ref={inputRef}
                                style={{ resize: "none" }}
                                onChange={e => setMessage(e.target.value)}
                            />
                            <Button variant="dark" className="rounded-0" onClick={() => {
                                try {
                                    connection?.invoke("Send", stores?.user.user?.id ?? 0, message);
                                    if (inputRef.current !== null)
                                        inputRef.current.value = "";
                                    setMessage('')
                                } catch (ex) { }
                            }}>Отправить</Button>
                        </div>
                    </main>
                </Col>
            </Row>
        </>
    )
}

export default Chat;