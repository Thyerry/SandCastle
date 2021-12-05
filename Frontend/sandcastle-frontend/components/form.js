import React, { useState } from "react";
import Button from "@material-tailwind/react/Button";
import Router from 'next/router'

export default function Form() {
    const [error, setError] = useState('');

    const joinSession = async event => {
        event.preventDefault()
        const sessionCode = event.target.inputSession.value;
        const session_endpoint = `localhost:2525/Jogo'/${sessionCode}`;

        const res = await fetch(session_endpoint, {
            headers : {
                'Content-Type': 'application/json',
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Origin, X-Requested-With, Content-Type, Accept, Authorization"
            },
            method: 'GET'
        })
        const result = await res.json()
        if(!("erro" in result)) {
            localStorage.setItem("session", sessionCode)
            Router.push(`game-session`)
        }
        setError(res.erro)
    }

    return (
        <form onSubmit={joinSession}>
            <div className="flex-col justify-center items-center"> 
                <input placeholder="Existing Session?" id="inputSession" name="inputSession" type="text" required className="border-b-2 flex"/>
                <Button block={true} size="sm"  type="submit"> Join Session! </Button>
            </div>
            {error && <p className="text-red-700"> {error}</p>}
        </form>
    )
}