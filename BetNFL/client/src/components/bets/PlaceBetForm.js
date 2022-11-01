import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom"
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import { getBetById } from "../../modules/betManager";
import { postUserProfileBet } from "../../modules/userProfileBetManager";

export default function PlaceBetForm() {
    const [bet, setBet] = useState();
    const [userProfileBet, setUserProfileBet] = useState();
    const { betId } = useParams();
    const navigate = useNavigate();

    const formatOdds = (odds) => {
        if (odds > 0) {
            return "+" + odds;
        }
        return odds;
    }

    useEffect(() => {
        getBetById(betId).then((bet) => {
            setBet(bet);
            setUserProfileBet({
                betId: bet.id,
                side: null,
                betAmount: 0
            });
        });
    }, []);

    return (
        <div className="form-container">
            <h4 className="bet-form-header">
                {bet?.game?.awayTeam?.fullName} @ {bet?.game?.homeTeam?.fullName}, Week {bet?.game?.week}
            </h4>
            <div className="bet-info-row">
                <h5>{bet?.userProfile?.username}</h5>
                <h5>{bet?.betType?.name?.toUpperCase()}</h5>
            </div>
            <Form className="form-container" style={{width: "85%"}}>
                <FormGroup 
                    tag="fieldset"
                    onChange={(e) => {
                        const copy = {...userProfileBet};
                        copy.side = parseInt(e.target.value);
                        setUserProfileBet(copy);
                    }}
                >
                    <h5>Select Your Side</h5>
                    <FormGroup check>
                        <Label check>
                        <Input type="radio" name="radio1" value={1}/>{' '}
                            {bet?.game?.awayTeam?.fullName} {formatOdds(bet?.awayTeamOdds)}
                        </Label>
                    </FormGroup>
                    <FormGroup check>
                        <Label check>
                        <Input type="radio" name="radio1" value={2}/>{' '}
                            {bet?.game?.homeTeam?.fullName} {formatOdds(bet?.homeTeamOdds)}
                        </Label>
                    </FormGroup>
                </FormGroup>
                <FormGroup>
                    <Label for="betAmount" style={{fontWeight: "bold"}}>Enter Bet Amount</Label>
                    <Input 
                        type="number" 
                        name="betAmount" 
                        placeholder="$0.00" 
                        onChange={(e) => {
                            const copy = {...userProfileBet};
                            copy.betAmount = parseFloat(parseFloat(e.target.value).toFixed(2));
                            setUserProfileBet(copy);
                        }}
                    />
                </FormGroup>
                <Button
                    onClick={() => {
                        if (!userProfileBet.side) {
                            window.alert("Error: Must select a side");
                            return;
                        } else if (!userProfileBet.betAmount) {
                            window.alert("Error: Must enter an amount to bet");
                            return;
                        } else if (userProfileBet.betAmount >= 10000000) {
                            window.alert("Error: Maximum bet amount exceeded");
                            return;
                        } else if (userProfileBet.betAmount < 1) {
                            window.alert("Error: Minimum bet amount is $1.00");
                            return;
                        }
                        
                        postUserProfileBet(userProfileBet).then(() => navigate("/openBets"))
                    }}
                >Place Bet</Button>
            </Form>
        </div>
    )
}