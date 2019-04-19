<?php

require("class.phpmailer.php");
error_reporting(E_ALL);
ini_set('display_errors', '1');


    $data = file_get_contents("php://input");
    $postData = json_decode($data);
    $sendto = $postData->sendto;
    $subject = $postData->subject;
    $body = $postData->body;


$mail = new PHPMailer();

$mail->CharSet = "UTF-8";

$mail->IsSMTP();
$mail->Host = "mi3-wss5.a2hosting.com";

$mail->SMTPAuth = true;
$mail->SMTPSecure = "ssl";
$mail->Port = 465;
$mail->Username = "web@promo-tekstil.com";
$mail->Password = "vMap90#3";

$mail->From = "web@promo-tekstil.com";
$mail->FromName = "Promo-Tekstil.com";
//$mail->FromName = $_POST['fromName'];
//$mail->AddAddress("mirza.hodzic.ri@gmail.com");
//$sendto = $_POST['sendto'];
$mail->AddAddress($sendto);
//$mail->AddReplyTo("mail@mail.com");

$mail->IsHTML(true);

$mail->Subject = $subject;
//$mail->Subject = "Ovo je mail poslan preko mi3-wss5.a2hosting.com SMTP servera sa SSL-om";
$mail->Body = $body;
//$mail->Body = "Ovo je mail poslan preko mi3-wss5.a2hosting.com SMTP servera sa SSL-om";
//$mail->AltBody = "This is the body in plain text for non-HTML mail clients";

if(!$mail->Send())
{
echo "Mail nije bilo moguće poslati.<p>";
echo "Mailer Error: " . $mail->ErrorInfo;
exit;
}

echo "Mail je uspješno poslan";

?>