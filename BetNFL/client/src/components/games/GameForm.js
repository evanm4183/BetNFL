import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { Form, FormGroup, Label, Input, Button } from 'reactstrap';
import "../../styles/form-styles.css";
import { getTeams } from "../../modules/teamManager";
import { getSiteTime } from "../../modules/siteTimeManager";

export default function GameForm() {
    const [teams, setTeams] = useState([]);
    const [game, setGame] = useState({});

    useEffect(() => {
        getTeams().then((teams) => {
            setTeams(teams);
        }).then(
            getSiteTime().then((siteTime) => {
                setGame(
                    {
                        homeTeamId: 0,
                        awayTeamId: 0,
                        week: siteTime.currentWeek,
                        year: siteTime.currentYear
                    }
                )
            }
        ));
    }, []);

    return (
        <Form className="form-container" style={{width: "50%", marginTop: "1%"}}>
            <FormGroup>
                <Label for="homeTeam">Home Team</Label>
                <Input type="select" name="homeTeam" id="home-team" onChange={(e) => {
                    const homeTeamId = parseInt(e.target.value);

                    if (homeTeamId === game.awayTeamId) {
                        window.alert("Error: The same team cannot be selected for both the home and away team");
                        return;
                    }

                    const copy = {...game}
                    copy.homeTeamId = homeTeamId;
                    setGame(copy);
                }}>
                    <option id="team--0" value={0}>Select a Team...</option>
                    {
                        teams.map((team) => {
                            return <option 
                                key={`team--${team.id}`} 
                                value={team.id}
                            >{team.fullName}</option>
                        })
                    }
                </Input>
            </FormGroup>
            <FormGroup>
                <Label for="awayTeam">Away Team</Label>
                <Input type="select" name="awayTeam" id="away-team" onChange={(e) => {
                    const awayTeamId = parseInt(e.target.value);

                    if (awayTeamId === game.homeTeamId) {
                        window.alert("Error: The same team cannot be selected for both the home and away team");
                        return;
                    }

                    const copy = {...game}
                    copy.awayTeamId = awayTeamId;
                    setGame(copy);
                }}>
                    <option id="team--0">Select a Team...</option>
                    {
                        teams.map((team) => {
                            return <option 
                                key={`team--${team.id}`}
                                value={team.id}
                            >{team.fullName}</option>
                        })
                    }
                </Input>
            </FormGroup>
            <FormGroup>
                <Label for="exampleEmail">Current Week</Label>
                <Input 
                    type="number" 
                    name="currentWeek" 
                    id="current-week" 
                    defaultValue={game.week}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.week = parseInt(e.target.value);
                        setGame(copy);
                    }}
                /> 
            </FormGroup>
            <FormGroup >
                <Label for="exampleEmail">Current Year</Label>
                <Input 
                    type="number" 
                    name="currentYear" 
                    id="current-year" 
                    defaultValue={game.year}
                    onChange={(e) => {
                        const copy = {...game};
                        copy.year = parseInt(e.target.value);
                        setGame(copy);
                    }} 
                /> 
            </FormGroup>
            <Button onClick={() => {
                if (game.homeTeamId === 0 || game.awayTeamId === 0) {
                    window.alert("Error: Must select a home and away team");
                    return;
                }
            }}>Submit</Button>
        </Form>
    );
}