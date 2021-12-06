import Head from 'next/head'
import styles from '../styles/Home.module.css'
import Form from '../components/form'
import Button from "@material-tailwind/react/Button";
import Router from 'next/router'
import { useState } from 'react';

export default function Home() {

  const [error, setError] = useState('');


  const createNewGameSession = async () => {
    const randHex =  [...Array(24)].map(() => Math.floor(Math.random() * 16).toString(16)).join('');
    let res = await fetch("http://127.0.0.1:2525/Jogo", {
      body: JSON.stringify({
        id: randHex
    }), headers : {
        'Content-Type': 'application/json'
    }, 
    method: 'POST'
    })

    let result = res.json()
    if(!("erro" in result)) {
      localStorage.setItem("session", randHex)
      Router.push(`game-session`) 
    }  
    setError(res.erro)
  }

  return (
    <div className={styles.container}>
      <Head>
        <title>Sandcastle</title>
        <meta name="description" content="Sandcastle Rpg Support" />
        <link rel="icon" href="/favicon.ico" />
      </Head>

      <main className={styles.main}>
        <h1 className={styles.title}>
          Welcome to SandCaslte!
        </h1>
        <Form />
       <Button onClick={createNewGameSession}>Create New Session</Button>
       {error && <p className="text-red-700"> {error}</p>}

      </main>
    </div>
  )
}
