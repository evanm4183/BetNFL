import { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import "../../styles/form-styles.css";
import { getGameById, setScore } from "../../modules/gameManager";

export default function GameEditForm() {
    const [game, setGame] = useState({});
    const { gameId } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        getGameById(gameId).then((game) => {
            setGame(game);
        });
    }, []);

    return (
        <Form className="form-container" style={{width: "50%"}}>
            <FormGroup>
                <Label for="awayTeam">{game?.awayTeam?.fullName} Score</Label>
                <Input 
                    type="number" 
                    name="awayScore"
                    defaultValue={game.awayTeamScore}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.awayTeamScore = parseInt(e.target.value);
                        setGame(copy);
                    }}
                />
            </FormGroup>
            <FormGroup>
                <Label for="awayTeam">{game?.homeTeam?.fullName} Score</Label>
                <Input 
                    type="number" 
                    name="awayScore"
                    defaultValue={game.homeTeamScore}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.homeTeamScore = parseInt(e.target.value);
                        setGame(copy);
                    }}
                />
            </FormGroup>
            <div className="button-row">
                <Button
                    onClick={(e) => {
                        setScore(game).then(() => {navigate("/")});
                    }}
                >Save Changes</Button>
                <Button 
                    onClick={(e) => {
                        const copy= {...game};
                        copy.awayTeamScore = null;
                        copy.homeTeamScore = null;
                        setGame(copy);
                    }}
                >Clear Scores</Button>
                <Button>Delete Game</Button>
            </div>
        </Form>
    );
}