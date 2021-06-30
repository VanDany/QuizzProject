var i = 1;
function addQuestionLine() {
    i++;
   var title = document.getElementById("Title"+i);
   if (title.style.display == 'none') {
        title.style.display = 'block';
            this.innerHTML = 'Hide';
        } else {
        title.style.display = 'none';
            this.innerHTML = 'Show';
    }

    var question = document.getElementById("Question" + i);
    if (question.style.display == 'none') {
        question.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        question.style.display = 'none';
        this.innerHTML = 'Show';
    }

    var vrai = document.getElementById("Vrai" + i);
    if (vrai.style.display == 'none') {
        vrai.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        vrai.style.display = 'none';
        this.innerHTML = 'Show';
    }

    var vrailabel = document.getElementById("VraiLabel" + i);
    if (vrailabel.style.display == 'none') {
        vrailabel.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        vrailabel.style.display = 'none';
        this.innerHTML = 'Show';
    }

    var fauxlabel = document.getElementById("FauxLabel" + i);
    if (fauxlabel.style.display == 'none') {
        fauxlabel.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        fauxlabel.style.display = 'none';
        this.innerHTML = 'Show';
    }

    var faux = document.getElementById("Faux" + i);
    if (faux.style.display == 'none') {
        faux.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        faux.style.display = 'none';
        this.innerHTML = 'Show';
    }

    var dashes = document.getElementById("Dashes" + i);
    if (dashes.style.display == 'none') {
        dashes.style.display = 'block';
        this.innerHTML = 'Hide';
    } else {
        dashes.style.display = 'none';
        this.innerHTML = 'Show';
    }

}

