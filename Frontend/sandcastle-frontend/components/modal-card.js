import Modal from "@material-tailwind/react/Modal";
import ModalHeader from "@material-tailwind/react/ModalHeader";
import ModalBody from "@material-tailwind/react/ModalBody";
import Card from "@material-tailwind/react/Card";
import CardBody from "@material-tailwind/react/CardBody";
import Button from "@material-tailwind/react/Button";
import { useState } from "react";


export default function ModalCard({showModal, setShowModal, setCards, cards}) {

    const [error, setError] = useState(false)

    const handleSubmit = async event => {
        event.preventDefault()
        const endpoint = 'http://localhost:2525/Fichas';
        let { Nome, Classe, Especificidades, Força, Inteligencia, Vigor} = event.target
        let newCard = {
            Nome : Nome.value,
            Classe : Classe.value,
            Especificidades : Especificidades.value,
            Força : Força.value,
            Inteligencia : Inteligencia.value,
            Vigor : Vigor.value,
            idJogo : localStorage.session
        }
        const res = await fetch(endpoint, {
            body: JSON.stringify(newCard), headers : {
                'Content-Type': 'application/json'
            }, 
            method: 'POST'
        })
        let response = await res.json()
        if(!("erro" in response)) {
            setCards([newCard, ...cards])
        } else {
            setError(response.erro)
        }
    }
    
    
    return (
        <Modal size="sm" setCards={setCards} cards={cards} active={showModal} toggler={() => setShowModal(false)}>
            <ModalHeader toggler={() => setShowModal(false)}>
                Novo Jogador
            </ModalHeader>
            <ModalBody>
            <Card>
            <CardBody>
                <form onSubmit={handleSubmit}> 
                    <input name="Nome" type="text" className="border-b-2 m-3" placeholder="Nome"/>
                    <input name="Classe" type="text" className="border-b-2 m-3" placeholder="Classe"/>
                    <input name="Especificidades" type="text" className="border-b-2 m-3" placeholder="Especificidades"/>
                    <input name="Força" type="number" className="border-b-2 m-3" placeholder="Força"/>
                    <input name="Inteligencia" type="number" className="border-b-2 m-3" placeholder="Inteligência"/>
                    <input name="Vigor" type="number" className="border-b-2 m-3" placeholder="Vigor"/>
                    <Button block={true} size="sm" color="gray" type="submit"> Registrar! </Button>
                    {error && <p className="text-red-700"> {error}</p>}
                </form>
            </CardBody>
        </Card>
            </ModalBody>
        </Modal>
    )

}
