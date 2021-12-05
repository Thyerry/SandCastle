import { useState } from "react";
import Button from "@material-tailwind/react/Button";



export default function Form() {
    const [error, setError] = useState('');

    const joinSession = async event => {
        event.preventDefault()
        const apiUrlForSession = '';
        const sessionCode = event.target.inputSession.value;
        const session_endpoint = `${apiUrlForSession}/${sessionCode}`;

        const res = await fetch(session_endpoint, {
            headers : {
                'Content-Type': 'application/json'
            },
            method: 'GET'
        })
        console.log(res)
        // const result = await res.json()
        // if(!("error" in result)) {
        //     // redirect(`game-session/${sessionCode}`)
        // }
        setError(res.statusText)
    }

    return (
        <form onSubmit={joinSession}>
            <div className="flex-col justify-center items-center"> 
                <input placeholder="Existing Session?" id="inputSession" name="inputSession" type="text" required className="border-b-2 flex"/>
                <Button block="true" size="sm" type="submit"> Join Session! </Button>
            </div>
            {error && <p className="text-red-700"> {error}</p>}
        </form>
    )
}