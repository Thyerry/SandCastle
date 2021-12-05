import React from "react";
import Card from "@material-tailwind/react/Card";
import CardBody from "@material-tailwind/react/CardBody";
import CardHeader from "@material-tailwind/react/CardHeader";

export default function PlayerCard({attributes}) {
    return (
        <Card className="mt-14 bg-gradient-to-b from-gray-50 to-gray-500">
            <CardHeader color="gray" size="sm">
                <h2>{attributes.Nome}</h2>
                <p className="text-gray-50">{attributes.Classe}</p>
            </CardHeader>
            <CardBody>
                <p className="p-1 "> 
                    Especificidades: {attributes.Especificidades}
                </p>
                <p className="p-1"> 
                    Força: {attributes.Força}
                </p>
                <p className="p-1"> 
                    Inteligência: {attributes.Inteligencia}
                </p>
                <p className="p-1"> 
                    Vigor: {attributes.Vigor}
                </p>
            </CardBody>
        </Card>
    )
}
