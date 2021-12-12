import * as alt from 'alt';
import * as game from 'natives';

const range = 250;
let peds = [];

alt.onServer('Client:Pedcreator:spawnPed', (pedArray) => {
    peds = JSON.parse(pedArray);
});

alt.setInterval(()=> {
    if (peds == null || peds.length == 0) return;
    const own_pos = alt.Player.local.pos

    peds.forEach(i => {

        const dist = game.getDistanceBetweenCoords(
            i.posX, 
            i.posY, 
            i.posZ,
            own_pos.x,
            own_pos.y,
            own_pos.z,
            false
        )

        if (dist > range && i.handle != 0) {
            game.setEntityInvincible(i.handle, false);
            game.disablePedPainAudio(i.handle, false);
            game.freezeEntityPosition(i.handle, false);
            game.taskSetBlockingOfNonTemporaryEvents(i.handle, false);
            game.deletePed(i.handle)
            i.handle = 0

        } else if (dist <= range && i.handle == 0) {

            let modelHash = game.getHashKey(i.model);
            new Promise((resolve, reject) => {
                if (game.hasModelLoaded(modelHash)) {
                    return resolve();
                }
                game.requestModel(modelHash);
                const timer = alt.setInterval(() => {
                    if (game.hasModelLoaded(modelHash)) {
                        alt.clearInterval(timer);
                        return resolve();
                    }
                }, 10);
            }).then(() => {
                i.handle = game.createPed(4, modelHash, i.posX, i.posY, i.posZ, i.rotation, false, true);
                game.setEntityInvincible(i.handle, true);
                game.disablePedPainAudio(i.handle, true);
                game.freezeEntityPosition(i.handle, true);
                game.taskSetBlockingOfNonTemporaryEvents(i.handle, true);
            });
        }
    })
}, 2000)