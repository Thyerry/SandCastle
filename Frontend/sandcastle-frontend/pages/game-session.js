import { useEffect , useState} from "react"
import styles from '../styles/Home.module.css'
import PlayerCardList from '../components/player-card-list'
import Button from "@material-tailwind/react/Button";
import ModalCard from "../components/modal-card";

export default function Game() {
    const [cards ,setCards] = useState([])
    const [number ,setNumber] = useState(null)
    const [showModal, setShowModal] = useState(false);

    useEffect(async () => {
        let cards = await getAllPlayerCards()
        let res = await cards.json();
        // TODO trocar cards pra res 
        setCards([res, res])
    },[])

    // TODO - arrumar endpoint
    const getAllPlayerCards = async () => {
        return await fetch(`http://127.0.0.1:2525/Fichas/${localStorage.session}`, {
            headers: {
                'Content-Type': 'application/json',
            }
        })
    }

    const playDice = number => {
        let numero = Math.floor(Math.random() * number) + 1
        console.log(numero)
        setNumber(numero)
    }

    return (
        <div className="flex-row mt-12 ml-12 flex bg-gradient-to-b from-gray-500 to-gray-50 h-full">
            <div className="max-w-2xl flex flex-col">
                    <Button size="sm" color="blueGray"onClick={(e) => setShowModal(true)}> Nova Ficha </Button>
                {cards && <PlayerCardList cards={cards} />}
                <ModalCard showModal={showModal} setCards={setCards} cards={cards} setShowModal={setShowModal} />
            </div>
            <main className={styles.main}>
                <div className="flex-row flex"> 
                    <Button color="blueGray" onClick={() => playDice(3)}> d3 </Button>
                    <Button color="blueGray" onClick={() => playDice(6)}> d6 </Button>
                    <Button color="blueGray" onClick={() => playDice(12)}> d12 </Button>
                    <Button color="blueGray" onClick={() => playDice(20)}> d20 </Button>
                </div>
                <h1 className={styles.title}>{number && `Você Rolou um ${number}!`}</h1>

            </main>
        </div>
    )
}