import Head from 'next/head'
import Link from 'next/link'
import Image from 'next/image'
import styles from '../styles/Home.module.css'
import Form from '../components/form'
import Button from "@material-tailwind/react/Button";

export default function Home() {
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
        <Link href="api/hello"><Button size="sm" color="blueGray">Create New Session</Button></Link>
      
      </main>
    </div>
  )
}
